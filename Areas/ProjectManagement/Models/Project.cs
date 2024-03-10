using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Areas.ProjectManagement.Models

{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }




        [Required]
        [Display(Name = "Project Name")]
        [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
        public required string Name { get; set; }





        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }





        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }





        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }

        public List<ProjectTask>? Tasks { get; set; }


    }
}
