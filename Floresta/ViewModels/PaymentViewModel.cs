using Floresta.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Floresta.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Вкажіть локацію")]
        public string Title { get; set; }
        public string Lat { get; set; }
        [Required(ErrorMessage = "Поставте мітку на карті")]
        public string Lng { get; set; }
        public int MarkerId { get;set; }
        [Required(ErrorMessage = "Поле кількості місць обов’язкове")]
        [Range(1, int.MaxValue, ErrorMessage = "Введіть кількість місць більшу за {1}")]
        public int PlantCount { get; set; }
        [Required(ErrorMessage = "Виберіть саджанець для висадки")]
        public int SeedlingId { get; set; }
        public List<Seedling> Seedlings { get; set; }

    }
}
