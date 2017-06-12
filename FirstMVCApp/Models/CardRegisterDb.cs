using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FirstMVCApp.Models
{
public class CardRegisterDb:DbContext
{
    public CardRegisterDb() :base("CardRegisterDatabase")
    {
    }

    public DbSet<CardRegister> CardRegisters { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
}
}