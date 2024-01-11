using System;
namespace CodePulse.Models.Domain
{
	public class BlogPostImage
	{
		public Guid Id { get; set; }
		public string FileName { get; set; }
		public string FileExtension { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public DateTime DateCreated { get; set; }

	}
}

