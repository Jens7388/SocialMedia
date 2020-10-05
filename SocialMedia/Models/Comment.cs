using System;
using System.Collections.Generic;

namespace SocialMedia.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsEdited { get; set; }
        public DateTime Created { get; set; }

        public virtual Post Post { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
