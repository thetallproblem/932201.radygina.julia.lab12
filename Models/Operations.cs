using System.ComponentModel.DataAnnotations;

namespace Lab12.Models
{
    public enum Operations
    {
        [Display (Name = "+")]
        Sum,
        [Display (Name = "-")]
        Sub,
        [Display (Name = "*")]
        Mult,
        [Display (Name = "/")]
        Div
    }
}
