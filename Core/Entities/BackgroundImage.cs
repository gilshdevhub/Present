using Core.Entities.Configuration;
using Microsoft.AspNetCore.Http;

namespace Core.Entities;

public class BackgroundImage
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime Untill { get; set; }
    public string Name { get; set; }
    public string Decription { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public bool IsTempExists { get; set; }
}

public class BackgroundImageDto
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime Untill { get; set; }
    public string Name { get; set; }
    public string Decription { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public bool IsTempExists { get; set; }
    public Dictionary<string, byte[]> images { get; set; }
}



public class UploadBackGround
{
    public IFormFile FileToLoad { get; set; }
    public DateTime Untill { get; set; }
    public DateTime From { get; set; }
    public string Name { get; set; }
}

public class UploadBackUpadetDateGround
{
    public DateTime Untill { get; set; }
    public DateTime From { get; set; }
    public string Name { get; set; }
}

public class BGResponse
{
    public byte[] file { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}