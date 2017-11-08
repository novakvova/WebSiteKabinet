﻿using DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope _transaction;
        public void CommitTransaction()
        {
            this._transaction.Complete();
        }

        public void Dispose()
        {
            if(_transaction !=null)
                this._transaction.Dispose();
        }

        public void StartTransaction()
        {
            this._transaction=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
