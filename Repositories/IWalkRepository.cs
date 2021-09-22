using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetAllWalks();
        Walk GetWalkById(int id);
        void AddWalk(Walk walk);
        void UpdateWalk(Walk walk);
        void DeleteWalk(int walkId);
        //List<Walk> GetWalksByWalkerId(int walkerId);
    }
}
