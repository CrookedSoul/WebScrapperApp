using SQLite;

namespace WebScraperApp.LocalData.Entity
{
    [Table("UserBookmarkInfo")]
    public class UserBookmarkInfo : BaseInfo
    {
        [Column("UserID")]
        [NotNull]
        public int UserID { get; set; }
        [Column("LastReadChapter")]
        public int LastReadChapter { get; set; }
        [Column("BookName")]
        public string BookName { get; set; }
        [Column("BookSlug")]
        public string BookSlug { get; set; }
        [Column("BookAuthor")]
        public string BookAuthor { get; set; }
        [Column("BookSource")]
        public string BookSource { get; set; }
        [Column("BookStatus")]
        public string BookStatus { get; set; }
        [Column("BookDescription")]
        public string BookDescription { get; set; }
        [Column("BookTags")]
        /// Ovo ce biti Tag1, Tag2, Tag3 zasad pa moze Split
        public string BookTags { get; set; }
        [Column("BookChapterListJson")]
        /// Ovo ce biti Json Serialize List<Chapter.ItemModel>
        public string BookChapterListJson { get; set; }
    }
}
