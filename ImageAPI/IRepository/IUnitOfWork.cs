using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageAPI.Entities;

namespace ImageAPI.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<ImageDetail> image { get;}

    }
}
