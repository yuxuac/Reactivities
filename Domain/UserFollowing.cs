namespace Domain
{
    public class UserFollowing
    {
        public string ObserverId { get; set; }

        /// <summary>
        /// 关注者
        /// </summary>
        /// <value></value>
        public virtual AppUser Observer { get; set; }
        public string TargetId { get; set; }

        /// <summary>
        /// 被关注者
        /// </summary>
        /// <value></value>
        public virtual AppUser Target { get; set; }
    }
}