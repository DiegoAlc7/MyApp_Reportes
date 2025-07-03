# Sistema de Inventario
Este proyecto esta diseñado para gestionar el inventario de una empresa y generar reportes. A continuación, se detallan los pasos para configurar y utilizar la aplicación web.

## **Instrucciones de Uso**

### **1. Configuración de la Base de Datos**
1. Abre el archivo `appsettings.json` y modifica la cadena de conexión:
   - En el campo `Server=TUSERVIDOR`, ingresa el nombre de tu servidor de SQL Server.
   - Si ya tienes una base de datos llamada `InventarioDB` en tu sistema de gestión de bases de datos (SGBD), cambia el nombre de la base de datos en la cadena de conexión para evitar conflictos.

2. Abre la **Consola del Administrador de Paquetes** en Visual Studio 2022 y ejecuta el siguiente comando:
   ```bash
   Update-Database
   ```
   Esto creará y actualizará automáticamente la base de datos en tu servidor SQL Server.

   > **Nota:** Si el comando no funciona, elimina la migración existente y crea una nueva ejecutando:
   ```bash
   Remove-Migration
   Add-Migration InitialCreate
   Update-Database
   ```
   También verifica que la cadena de conexión esté correctamente configurada.

### **2. Inserción de Datos Iniciales**
1. En tu servidor SQL Server, abre una nueva consulta (`New Query`) y ejecuta el siguiente script para insertar datos iniciales en la base de datos:

```sql
USE InventarioDB --Reemplaza el nombre de tu BD si lo cambiaste;

-- Insertar roles
INSERT INTO Roles VALUES ('Administrador');
INSERT INTO Roles VALUES ('Operador');

-- Insertar usuarios
-- Clave: admin123
INSERT INTO Usuarios VALUES('Admin', 'admin@gmail.com', '$2a$12$g29HYgNkU3FjOORi3OXYyOrQ9pQFN8rGrkE3pDw6bFxk8WLlNkoqy', 1);

-- Clave: operador123
INSERT INTO Usuarios VALUES('Operador', 'operador@gmail.com', '$2a$11$GDEMBC/Yp4lUgHAzx9shZeIZ2b5yUUDAvZfAjacH/ULRIrhiz7V/i', 2);

-- Insertar artículos
INSERT INTO Artículos (Código, Nombre, Categoría, Estado, Ubicación)
VALUES 
    ('LAP-001', 'Laptop Dell XPS 15', 'Tecnología', 'Disponible', 'Almacén A'),
    ('PROY-001', 'Proyector Epson EB-U05', 'Tecnología', 'Disponible', 'Almacén B'),
    ('MES-001', 'Mesa plegable', 'Mobiliario', 'Disponible', 'Almacén C'),
    ('SIL-001', 'Silla ergonómica', 'Mobiliario', 'Disponible', 'Oficina 201'),
    ('CAM-001', 'Cámara Canon EOS R', 'Fotografía', 'Disponible', 'Taller'),
    ('IMP-001', 'Impresora HP LaserJet Pro', 'Tecnología', 'Disponible', 'Almacén A'),
    ('TAB-001', 'Tablet Samsung Galaxy Tab S8', 'Tecnología', 'Disponible', 'Oficina 102'),
    ('ESC-001', 'Pizarra blanca magnética', 'Mobiliario', 'Disponible', 'Sala de Reuniones 3'),
    ('AUD-001', 'Audífonos Sony WH-1000XM5', 'Audio', 'Disponible', 'Almacén B'),
    ('KIT-001', 'Kit de iluminación LED', 'Fotografía', 'Disponible', 'Taller');
```

### **3. Inicio de Sesión**
1. Ejecuta la aplicación web. Serás redirigido a una página de inicio de sesión.
2. Usa las siguientes credenciales para acceder:

   **Administrador:**
   - Correo: `admin@gmail.com`
   - Contraseña: `admin123`

   **Operador:**
   - Correo: `operador@gmail.com`
   - Contraseña: `operador123`

   > **Nota:** Verifica que los datos del script SQL se hayan insertado correctamente antes de intentar iniciar sesión.


### **4. ¡Listo!**
Ahora puedes utilizar el sistema para gestionar el inventario, registrar préstamos, generar reportes y más.

## **Notas Adicionales**
- Si encuentras algún problema durante la configuración, revisa la consola de errores o los registros del sistema para obtener más detalles.
- Asegúrate de tener instaladas las dependencias necesarias y las herramientas de desarrollo requeridas (por ejemplo, .NET SDK, SQL Server).
