namespace APIToDoList.Model
{
    public partial class DataBaseContext : Model.DbdebianContext
    {
        public static Model.DbdebianContext _context { get; } = new Model.DbdebianContext();
    }
}
