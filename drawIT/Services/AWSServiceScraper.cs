using drawIT.Database;
using drawIT.Models;
using drawIT.Services.Interfaces;
using MongoDB.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace drawIT.Services
{
    public class AWSServiceScraper : IHostedService, IDisposable, IAWSServiceScraper
    {
        private Timer? _timer;
        private readonly IDbContext _context;
        private readonly ILogger<AzureServiceScraper> _logger;

        public AWSServiceScraper(ILogger<AzureServiceScraper> logger, IDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("AWS service scraper is starting.");

            _timer = new Timer(async _ => await StoreScrapedAWSServices(), null, TimeSpan.Zero, TimeSpan.FromDays(7));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("AWS service scraper is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task<List<AWSService>> GetAWSCloudServicesAsync()
        {
            var awsServices = new List<AWSService>();
            IWebDriver driver = new ChromeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl("https://aws.amazon.com/products/");
            while (true)
            {
                wait.Until(drv => drv.FindElements(By.CssSelector("div.m-headline")).Count > 0);
                var services = driver.FindElements(By.CssSelector("div.m-headline"));
                var categories = driver.FindElements(By.CssSelector("div.m-category"));

                for (int i = 0; i < services.Count; i++)
                {
                    var service = services[i];
                    string category = "";
                    if (i < categories.Count)
                    {
                        category = categories[i].Text;
                    }

                    var sanitizedInput = service.Text.Replace("(Preview)", "").Trim();
                    awsServices.Add(new AWSService { Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(), Name = sanitizedInput, Category = category });
                }

                try
                {
                    var nextButton = wait.Until(drv => drv.FindElement(By.CssSelector("a[aria-label='Next Page']")));
                    string oldPageNumber = driver.FindElement(By.CssSelector("a[aria-current='true']")).Text;

                    if (nextButton.Displayed && nextButton.Enabled)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nextButton);
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextButton);
                        wait.Until(drv =>
                        {
                            try
                            {
                                return drv.FindElement(By.CssSelector("a[aria-current='true']")).Text != oldPageNumber;
                            }
                            catch (StaleElementReferenceException)
                            {
                                return true;
                            }
                        });
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Issue ");
                    break;
                }
            }

            driver.Close();

            return awsServices;
        }

        public async Task<List<AWSService>> StoreScrapedAWSServices()
        {
            var awsServices = await GetAWSCloudServicesAsync();
            _logger.LogInformation($"Retrieved {awsServices.Count} services from scraper.");

            try
            {
                foreach (var awsService in awsServices)
                {
                    _logger.LogInformation($"Processing service: {awsService.Name}, category: {awsService.Category}");

                    var filterByName = Builders<AWSService>.Filter.Eq(s => s.Name, awsService.Name);
                    var existingServices = await _context.AWSServices.Find(filterByName).ToListAsync();

                    if (!existingServices.Any())
                    {
                        await _context.AWSServices.InsertOneAsync(awsService);
                    }
                    else
                    {
                        var isServiceWithSameCategoryExists = existingServices.Any(s => s.Category == awsService.Category);
                        if (!isServiceWithSameCategoryExists)
                        {
                            await _context.AWSServices.InsertOneAsync(awsService);
                        }
                        else
                        {
                            _logger.LogInformation($"Service with name {awsService.Name} and category {awsService.Category} already exists.");
                        }
                    }
                }

                var countAfterProcessing = await _context.AWSServices.CountDocumentsAsync(_ => true);
                _logger.LogInformation($"Stored {countAfterProcessing} services.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing down records");
            }

            return awsServices;
        }

    }
}
