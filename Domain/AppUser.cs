using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string Bio { get; set; }

        public virtual ICollection<UserActivity> UserActivities { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        /// <summary>
        /// 当前用户关注的人
        /// </summary>
        /// <value></value>
        public virtual ICollection<UserFollowing> Followings { get; set; }
        
        /// <summary>
        /// 关注当前用户的人
        /// </summary>
        /// <value></value>
        public virtual ICollection<UserFollowing> Followers { get; set; }
    }
}