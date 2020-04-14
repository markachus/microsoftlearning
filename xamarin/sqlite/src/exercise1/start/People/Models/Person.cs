

using SQLite;

namespace People.Models
{
    [Table("people")]
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique, MaxLength(250)]
        public string Name { get; set; }
    }
}
