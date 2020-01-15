using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmurfWalletOW.Factory.Interface
{
    public interface IFactory<T>
    {
        ViewModelBase Get(T val,string message);
    }
}
