using System.Collections.Generic;
using BackEnd.Models;

namespace BackEnd.Data;

public interface IPlatformRepo
{
    bool SaveChanger();

    IEnumerable<Platform> GetAllPLatforms();

    Platform GetPlatformById(int id);

    void CreatePlatform(Platform platform);

}