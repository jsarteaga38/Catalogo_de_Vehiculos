# 📖 Manual de Usuario

## Proyecto: Catálogo de Vehículos
**Materia:** Herramientas de Programación  
**Institución:** Institución Universitaria Pascual Bravo  
**Integrantes:** Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya

---

## 1. Requisitos del Sistema

| Requisito | Detalle |
|-----------|---------|
| Sistema Operativo | Windows 10 o superior |
| Framework | .NET 10.0 o superior |
| Base de Datos | SQL Server LocalDB |
| Visual Studio | 2022 o superior |
| RAM mínima | 4 GB |
| Espacio en disco | 500 MB |

---

## 2. Instalación y Configuración

### Paso 1 — Clonar el repositorio
    git clone https://github.com/jsarteaga38/Catalogo_de_Vehiculos.git

### Paso 2 — Crear la base de datos
Abrir SQL Server Management Studio o Azure Data Studio y ejecutar:

    CREATE DATABASE CatalogoVehiculos;
    GO

    USE CatalogoVehiculos;
    GO

    CREATE TABLE Vehiculos (
        Id             INT IDENTITY(1,1) PRIMARY KEY,
        Tipo           NVARCHAR(50)  NOT NULL,
        Marca          NVARCHAR(100) NOT NULL,
        Modelo         NVARCHAR(100) NOT NULL,
        Año            INT           NOT NULL,
        Precio         FLOAT         NOT NULL,
        Color          NVARCHAR(50),
        Caracteristica NVARCHAR(100)
    );

### Paso 3 — Ejecutar la aplicación
    cd "Catalogo de Vehiculo"
    dotnet run

---

## 3. Pantalla Principal

Al iniciar la aplicación se muestra la ventana principal con las siguientes secciones:

    +------------------------------------------+
    |  Sistema Empresarial - Catálogo Vehículos |
    +------------------------------------------+
    |  [Registro de Vehículo]  [Búsqueda]       |
    |                          [📊 Dashboard]   |
    |                          [📥 Excel]       |
    +------------------------------------------+
    |  [Listado de Vehículos]                   |
    +------------------------------------------+
    |  Barra de estado: Total | Flota | Deprec. |
    +------------------------------------------+

---

## 4. Registrar un Vehículo

### Pasos:

1. Seleccionar el **tipo de vehículo** en el ComboBox
   - Automovil
   - Camion
   - Motocicleta

2. Completar los campos obligatorios:
   - **Marca** — nombre del fabricante (ej: Toyota)
   - **Modelo** — nombre del modelo (ej: Corolla)
   - **Año** — año de fabricación entre 1900 y el año actual
   - **Precio** — precio base en pesos, mayor a 0
   - **Color** — color del vehículo

3. Completar el campo **Característica** según el tipo:
   - Automovil → número de puertas (ej: 4)
   - Camion → peso de carga en toneladas (ej: 5.5)
   - Motocicleta → tipo (ej: Deportiva, Scooter, Enduro)

4. Presionar el botón **"Registrar"**

### Validaciones automáticas:
Si algún campo es incorrecto, aparecerá un mensaje en rojo debajo del campo:

    ⚠ Seleccione un tipo de vehículo
    ⚠ La marca es obligatoria
    ⚠ El modelo es obligatorio
    ⚠ Año válido entre 1900 y 2026
    ⚠ Precio debe ser mayor a 0
    ⚠ Puertas debe ser un número entero

---

## 5. Buscar un Vehículo

1. Escribir la **marca** en el campo "Buscar por marca exacta"
2. Presionar **"Buscar"**
3. El resultado aparece en el listado

Si no se encuentra: aparece el mensaje "No se encontró ningún vehículo con esa marca."

---

## 6. Eliminar un Vehículo

1. Escribir la **marca** del vehículo a eliminar
2. Presionar **"Eliminar"**
3. Confirmar la eliminación en el diálogo
4. El listado se actualiza automáticamente

---

## 7. Mostrar Todos los Vehículos

Presionar el botón **"Mostrar"** para cargar todos los vehículos registrados en el listado.

---

## 8. Filtros Avanzados

La sección de filtros permite combinar criterios de búsqueda:

| Filtro | Descripción |
|--------|-------------|
| Tipo | Filtrar por Automovil, Camion o Motocicleta |
| Marca | Filtrar por texto parcial de marca |
| Año | Filtrar por año exacto |

### Pasos:
1. Seleccionar o escribir los filtros deseados
2. Presionar **"Filtrar"**
3. Para ver todos de nuevo presionar **"Todos"**

---

## 9. Dashboard de Estadísticas

Presionar el botón **"📊 Dashboard"** para abrir el panel de estadísticas:

### Tarjetas de conteo:
- 🚗 Total de Automóviles
- 🚛 Total de Camiones
- 🏍 Total de Motocicletas
- 🚘 Total general

### Tarjetas financieras:
- 💰 Valor actual de la flota (precio - depreciación)
- 📉 Depreciación total acumulada
- 🔧 Costo de mantenimiento total

### Gráfica de barras:
Visualización gráfica de la distribución por tipo de vehículo.

---

## 10. Exportar a Excel

1. Presionar el botón **"📥 Exportar Excel"**
2. El archivo se genera automáticamente en el **Escritorio**
3. El nombre del archivo incluye fecha y hora:
   `Catalogo_Vehiculos_20260521_143022.xlsx`

### Contenido del archivo Excel:
- Encabezado con título y fecha de generación
- Columnas: Tipo, Marca, Modelo, Año, Precio, Color, Característica, Depreciación, Valor Actual
- Fila de totales con fórmulas automáticas
- Formato profesional con colores y estilos

---

## 11. Cálculos Automáticos

El sistema calcula automáticamente para cada vehículo:

### Depreciación
| Tipo | Fórmula |
|------|---------|
| Automovil | Precio × 10% × Antigüedad |
| Camion | Precio × 12% × Antigüedad |
| Motocicleta | Precio × 8% × Antigüedad |

La depreciación no puede superar el 100% del precio base.

### Costo de Mantenimiento
| Tipo | Fórmula |
|------|---------|
| Automovil | Precio × 5% |
| Camion | Precio × 8% |
| Motocicleta | Precio × 3% |

### Valor Actual
    Valor Actual = MAX(0, Precio - Depreciación)

---

## 12. Barra de Estado

En la parte inferior de la ventana principal se muestra en tiempo real:

    Total vehículos: 5 | Valor flota: $45,000,000 | Depreciación total: $12,000,000

Esta información se actualiza automáticamente cada vez que se registra o elimina un vehículo.

---

## 13. Mensajes del Sistema

| Mensaje | Descripción |
|---------|-------------|
| "Vehículo registrado correctamente" | Registro exitoso |
| "Vehículo eliminado correctamente" | Eliminación exitosa |
| "No se encontró ningún vehículo" | Búsqueda sin resultados |
| "Archivo exportado en: [ruta]" | Excel generado exitosamente |
| "Error inesperado: [detalle]" | Error de conexión o datos |

---

## 14. Solución de Problemas

### Error de conexión a base de datos
Verificar que SQL Server LocalDB esté instalado y la base de datos `CatalogoVehiculos` exista.

    Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CatalogoVehiculos

### El archivo Excel no se genera
Verificar que tenga permisos de escritura en el Escritorio.

### La aplicación no inicia
Verificar que .NET 10.0 esté instalado:

    dotnet --version