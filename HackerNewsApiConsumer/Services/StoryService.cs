using HackerNewsApiConsumer.Interfaces;
using HackerNewsApiConsumer.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace HackerNewsApiConsumer.Services
{
	public class StoryService : IStoryService
	{
		private readonly HttpClient _httpClient;
		private readonly IMemoryCache _memoryCache;
		private const string baseApiUrl = "https://hacker-news.firebaseio.com/v0/";
		public StoryService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
		{
			_httpClient = httpClientFactory.CreateClient("apiHttpClient");
			_httpClient.BaseAddress = new Uri(baseApiUrl);
			_memoryCache = memoryCache;
		}
		public async Task<IEnumerable<Story>> GetStories()
		{
			var stories = new List<Story>();
			var memoryCacheKey = "storiesMemoryCacheKey";
			if (!_memoryCache.TryGetValue(memoryCacheKey, out stories))
			{
				var httpResponse = await _httpClient.GetAsync("newstories.json");
				if (httpResponse.IsSuccessStatusCode)
				{
					var storyIds = JsonSerializer.Deserialize<List<int>>(await httpResponse.Content.ReadAsStreamAsync());
					if (storyIds?.Count > 0)
					{
						stories = (await Task.WhenAll(storyIds.Select(GetStory).AsParallel())).Where(s => s != null).ToList();
					}
				}
				var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

				_memoryCache.Set(memoryCacheKey, stories, memoryCacheEntryOptions);
			}
			return stories;
		}

		public async Task<Story> GetStory(int id)
		{
			var story = new Story();
			var httpResponse = await _httpClient.GetAsync($"item/{id}.json");
			if (httpResponse.IsSuccessStatusCode)
			{
				story = JsonSerializer.Deserialize<Story>(await httpResponse.Content.ReadAsStringAsync());
			}
			return story;
		}
	}
}
