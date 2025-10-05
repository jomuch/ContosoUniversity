using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml.Linq;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            if (context.Students.Any()) return;
        }

        public static void InitializeFromXml(SchoolContext context, string xmlFile)
        {
            if (context.Students.Any()) return;
            XDocument doc = XDocument.Load(xmlFile);
        }
    }
}
