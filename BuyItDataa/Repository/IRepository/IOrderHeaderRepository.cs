﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuyItData.Models;

namespace BuyItData.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    }
}
