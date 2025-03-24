using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPIPRoject.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string  ImageURL { get; set; }
        
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        
       // [JsonIgnore]
        public Department? Department { get; set; }
    }
}
