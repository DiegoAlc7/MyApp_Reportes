using Microsoft.EntityFrameworkCore;
using MyApp.DataAccess.Data;
using MyApp.DataAccess.IRepositories;
using MyApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccess.Repositories
{
    public class PréstamoRepository : IPréstamoRepository
    {
        private readonly ApplicationDbContext _context;

        public PréstamoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener préstamo por ID
        public Préstamo ObtenerPorId(int id)
        {
            return _context.Préstamos
                .Include(p => p.Usuario)
                .Include(p => p.Artículo)
                .FirstOrDefault(p => p.Id == id);
        }

        // Obtener todos los préstamos
        public IEnumerable<Préstamo> ObtenerTodos()
        {
            return _context.Préstamos
                .Include(p => p.Usuario)
                .Include(p => p.Artículo)
                .ToList();
        }

        // Filtrar préstamos por estado
        public IEnumerable<Préstamo> FiltrarPorEstado(string estado)
        {
            return _context.Préstamos
                .Include(p => p.Usuario)
                .Include(p => p.Artículo)
                .Where(p => p.Estado == estado)
                .ToList();
        }

        // Filtrar préstamos por usuario
        public IEnumerable<Préstamo> FiltrarPorUsuario(int usuarioId)
        {
            return _context.Préstamos
                .Include(p => p.Artículo)
                .Include(p => p.Usuario)
                .Where(p => p.UsuarioId == usuarioId)
                .ToList();
        }

        // Crear un nuevo préstamo
        public void Crear(Préstamo préstamo)
        {
            _context.Préstamos.Add(préstamo);
            _context.SaveChanges();
        }

        // Actualizar un préstamo existente
        public void Actualizar(Préstamo préstamo)
        {
            _context.Préstamos.Update(préstamo);
            _context.SaveChanges();
        }

        // Eliminar un préstamo por ID
        public void Eliminar(int id)
        {
            var préstamo = _context.Préstamos.Find(id);
            if (préstamo != null)
            {
                _context.Préstamos.Remove(préstamo);
                _context.SaveChanges();
            }
        }
    }
}
