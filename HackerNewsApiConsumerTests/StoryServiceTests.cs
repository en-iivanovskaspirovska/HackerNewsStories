using HackerNewsApiConsumer.Interfaces;
using HackerNewsApiConsumer.Models;
using HackerNewsApiConsumer.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using System.Text.Json;
namespace HackerNewsApiConsumerTests;
public class StoryServiceTests
{
	private Mock<IHttpClientFactory> _httpClientFactoryMock;
	private Mock<HttpMessageHandler> _httpMessageHandlerMock;
	private IStoryService _storyService;
	private IMemoryCache _memoryCache;

	public StoryServiceTests()
	{
		_httpClientFactoryMock = new Mock<IHttpClientFactory>();
		_httpMessageHandlerMock = new Mock<HttpMessageHandler>();
		_memoryCache = new MemoryCache(new MemoryCacheOptions());
	}

	[Fact]
	public void GetStories_StoriesAvailableInCache_ReturnsStoriesFromCache()
	{
		//Arrange
		var apiStories = new List<Story> {
			new Story { Id = 1, Title = "First Story" },
			new Story { Id = 2, Title = "Second Story" },
			new Story { Id = 3, Title = "Third Story" }
		};

		_memoryCache.Set("storiesMemoryCacheKey", apiStories, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60)));
		_httpClientFactoryMock.Setup(x => x.CreateClient("apiHttpClient")).Returns(new HttpClient()
		{
			BaseAddress = new Uri("https://someurl.com")
		});
		_storyService = new StoryService(_httpClientFactoryMock.Object, _memoryCache);

		//Act
		var storyResponse = _storyService.GetStories();

		//Assert
		Assert.NotNull(storyResponse);
		Assert.Equal(apiStories, storyResponse.Result.ToList());
	}
	
	[Fact]
	public void GetStory_StoryExists_ReturnsStory()
	{
		//Arrange
		var story = new Story { Id = 1, Title = "First Story" };

		//Act
		var storyResponse = CreateStoryServiceWithHttpClientAndMemoryCache(new StringContent(JsonSerializer.Serialize(story))).GetStory(25);

		//Assert
		Assert.NotNull(storyResponse);
		Assert.Equal(story.Id, storyResponse.Result.Id);
		Assert.Equal(story.Title, storyResponse.Result.Title);
	}

	[Fact]
	public void GetStory_StoryDoesNotExist_ReturnsEmptyStory()
	{
		//Arrange
		var emptyStory = new Story();

		//Act
		var storyResponse = CreateStoryServiceWithHttpClientAndMemoryCache(new StringContent(JsonSerializer.Serialize(emptyStory))).GetStory(25);

		//Assert
		Assert.NotNull(storyResponse);
		Assert.Equal(emptyStory.Id, storyResponse.Result.Id);
	}

	public IStoryService CreateStoryServiceWithHttpClientAndMemoryCache(StringContent stringContent)
	{
		_httpMessageHandlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>(
			"SendAsync",
			ItExpr.IsAny<HttpRequestMessage>(),
			ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage()
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = stringContent
			});
		_httpClientFactoryMock.Setup(x => x.CreateClient("apiHttpClient")).Returns(new HttpClient(_httpMessageHandlerMock.Object)
		{
			BaseAddress = new Uri("https://someurl.com")
		});
		_storyService = new StoryService(_httpClientFactoryMock.Object, _memoryCache);

		return _storyService;
	}
}