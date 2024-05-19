using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.ValidationModels
{
    public class Task
    {

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "Deadline is required")]
        [FutureDate(ErrorMessage = "Deadline must be in the future")]
        public DateTime Deadline { get; set; }

        public string Status { get; set; }

        public List<string> Categories { get; set; }

        public List<string> Tags { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date;
            if (DateTime.TryParse(value?.ToString(), out date))
            {
                return date >= DateTime.Today;
            }
            return false;
        }
    }
}
