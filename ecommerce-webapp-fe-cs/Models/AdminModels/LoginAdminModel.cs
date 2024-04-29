﻿using System.ComponentModel.DataAnnotations;

namespace ecommerce_webapp_fe_cs.Models.AdminModels;

public class LoginAdminModel
{
    [Required, EmailAddress, Display(Name = "Email")]
    public string Email { get; set; }

    [Required, DataType(DataType.Password), Display(Name = "Password")]
    public string Password { get; set; }
}