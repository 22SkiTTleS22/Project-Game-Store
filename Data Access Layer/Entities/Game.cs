using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Data_Access_Layer.Entities
{
    public class Game
    {
        [HiddenInput(DisplayValue = false)]
        public int GameId { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название игры")]
        public String Name { get; set; }
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание игры")]
        public string Discription { get; set; }
        [Display(Name = "Цена (в рублях)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal Price { get; set; }
        [Display(Name = "Скидка")]
        [Range(0, 100, ErrorMessage = "Пожалуйста, введите положительное или нулевое значение для скидки")]
        public int Discount { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        [Display(Name = "Активный ключ, введите новый ключ для добавления")]
        [Required(ErrorMessage = "Пожалуйста, введите новый ключ для игры")]
        public string ActiveKey { get; set; }
     
        public ICollection<Key> Keys { get; set; }
        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Пожалуйста, укажите категорию игры")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Display(Name ="Количество ключей в Базе Данных")]
        public int? CountKeys { get; set; }
        ICollection<OrdGame> OrdGames { get; set; }

    }
}
