﻿namespace OrderFetcher.Application.Responses.GPT;

public class OrderItemResponse
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}