﻿namespace OrderFetcher.Application.Dtos;

public class OrderItemDto
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}