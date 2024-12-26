using System.ComponentModel.DataAnnotations;

namespace Lab12.Models
{
    public class Calculate
    {
        [Required(ErrorMessage = "Нужно ввести число")]
        public double firstNum {  get; set; }

		[Required(ErrorMessage = "Нужно ввести число")]
		public int secondNum { get; set; }
        public Operations operation { get; set; }
    }
}
