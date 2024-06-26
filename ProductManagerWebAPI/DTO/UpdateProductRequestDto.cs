﻿using System.ComponentModel.DataAnnotations;

namespace ProductManagerWebAPI.DTO;
public class UpdateProductRequest
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string StockKeepingUnit { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ImageURL { get; set; }

    [Required]
    public decimal Price { get; set; }
}