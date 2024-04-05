using HackerNewsApiConsumer.Models;

namespace HackerNewsApiConsumer.Interfaces
{
	public interface IStoryService
	{
		Task<IEnumerable<Story>> GetStories();
		Task<Story> GetStory(int id);
	}
}
