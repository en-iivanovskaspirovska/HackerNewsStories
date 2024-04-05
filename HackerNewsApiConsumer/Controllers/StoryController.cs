using HackerNewsApiConsumer.Interfaces;
using HackerNewsApiConsumer.Models;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApiConsumer.Controllers
{
	[ApiController]
	[Route("stories")]
	public class StoryController : ControllerBase
	{
		private readonly ILogger<StoryController> _logger;
		private readonly IStoryService _storyService;
		public StoryController(ILogger<StoryController> logger, IStoryService storyService)
		{
			_logger = logger;
			_storyService = storyService;
		}

		[HttpGet(Name = "GetStories")]
		public async Task<IEnumerable<Story>> Get()
		{
			return await _storyService.GetStories();
		}
	}
}
