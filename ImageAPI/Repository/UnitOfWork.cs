using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageAPI.Entities;
using ImageAPI.IRepository;
using MongoDB.Driver;

namespace ImageAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private const string databaseName = "image";

        private readonly IMongoCollection<ImageDetail> _imageContext;
        private IGenericRepository<ImageDetail> _image;



        public UnitOfWork(IMongoClient client)
        {
            IMongoDatabase db = client.GetDatabase(databaseName);
            _imageContext = db.GetCollection<ImageDetail>(nameof(ImageDetail));
        }

        public IGenericRepository<ImageDetail> image => _image ??= new GenericRepository<ImageDetail>(_imageContext);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
