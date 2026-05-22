# 🔄 Diagrama de Flujo de la Interfaz

## Proyecto: Catálogo de Vehículos
**Materia:** Herramientas de Programación  
**Institución:** Institución Universitaria Pascual Bravo  
**Integrantes:** Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya

---

## Flujo Principal de la Aplicación

    [INICIO]
        |
        ↓
    [Cargar FormPrincipal]
        |
        ↓
    [Inicializar CatalogoService]
        |
        ↓
    [Suscribir FormPrincipal como Observer]
        |
        ↓
    [Mostrar UI Principal]
        |
        ↓
    +---+---+---+---+---+
    |   |   |   |   |   |
    ↓   ↓   ↓   ↓   ↓   ↓
  [Reg][Mos][Bus][Eli][Das][Exp]
  trar trar car  minar hboa rtar

---

## Flujo: Registrar Vehículo

    [Usuario selecciona tipo en ComboBox]
        |
        ↓
    [Mostrar campo Característica según tipo]
        |
        Camion → "Peso de carga (Ton)"
        Automovil → "Cantidad de puertas"
        Motocicleta → "Tipo de motocicleta"
        |
        ↓
    [Usuario llena campos: Marca, Modelo, Año, Precio, Color, Característica]
        |
        ↓
    [Usuario presiona "Registrar"]
        |
        ↓
    [ValidarFormulario()]
        |
        ├── ¿Campos vacíos o inválidos?
        │       |
        │       ↓
        │   [Mostrar labels de error en rojo]
        │   [Resaltar campos con fondo rosa]
        │       |
        │       ↓
        │   [FIN — esperar corrección]
        │
        └── ¿Todo válido?
                |
                ↓
            [VehiculoFactory.Crear(tipo, datos...)]
                |
                ↓
            [CatalogoPresenter.RegistrarVehiculo()]
                |
                ↓
            [CatalogoService.AgregarVehiculo(vehiculo)]
                |
                ↓
            [VehiculoRepository.Agregar(vehiculo)] → SQL Server
                |
                ↓
            [CatalogoService.Notificar("Vehículo agregado")]
                |
                ↓
            [FormPrincipal.OnCatalogoActualizado()] ← Observer
                |
                ↓
            [CatalogoPresenter.CargarVehiculos()]
                |
                ↓
            [Actualizar DataGridView automáticamente]
                |
                ↓
            [LimpiarFormulario()]
                |
                ↓
            [Mostrar mensaje "Registrado correctamente"]
                |
                ↓
            [FIN]

---

## Flujo: Buscar Vehículo

    [Usuario escribe marca en txtBuscarMarca]
        |
        ↓
    [Usuario presiona "Buscar"]
        |
        ↓
    [CatalogoPresenter.BuscarVehiculo()]
        |
        ↓
    [CatalogoService.BuscarPorMarca(marca)]
        |
        ↓
    [VehiculoRepository.BuscarPorMarca(marca)] → SQL Server
        |
        ├── ¿Encontrado?
        │       |
        │       ↓
        │   [MostrarVehiculos(lista con 1 elemento)]
        │   [FIN]
        │
        └── ¿No encontrado?
                |
                ↓
            [Mostrar mensaje "Sin resultados"]
            [FIN]

---

## Flujo: Eliminar Vehículo

    [Usuario escribe marca en txtBuscarMarca]
        |
        ↓
    [Usuario presiona "Eliminar"]
        |
        ↓
    [CatalogoPresenter.EliminarVehiculo()]
        |
        ↓
    [CatalogoService.BuscarPorMarca(marca)]
        |
        ├── ¿No encontrado?
        │       |
        │       ↓
        │   [Mostrar "Sin resultados"]
        │   [FIN]
        │
        └── ¿Encontrado?
                |
                ↓
            [CatalogoService.EliminarVehiculo(vehiculo)]
                |
                ↓
            [VehiculoRepository.Eliminar(vehiculo)] → SQL Server
                |
                ↓
            [CatalogoService.Notificar("Vehículo eliminado")]
                |
                ↓
            [FormPrincipal.OnCatalogoActualizado()] ← Observer
                |
                ↓
            [Actualizar DataGridView automáticamente]
                |
                ↓
            [Mostrar "Eliminado correctamente"]
            [FIN]

---

## Flujo: Aplicar Filtros

    [Usuario selecciona filtros: Tipo, Marca, Año]
        |
        ↓
    [Usuario presiona "Filtrar"]
        |
        ↓
    [AplicarFiltros()]
        |
        ↓
    [CatalogoService.ObtenerTodos()]
        |
        ↓
    [Filtrar por Tipo si seleccionado]
        |
        ↓
    [Filtrar por Marca si escrita]
        |
        ↓
    [Filtrar por Año si escrito]
        |
        ↓
    [MostrarVehiculos(lista filtrada)]
        |
        ↓
    [Actualizar estadísticas en barra de estado]
        |
        ↓
    [FIN]

---

## Flujo: Dashboard

    [Usuario presiona "Dashboard"]
        |
        ↓
    [CatalogoService.ObtenerTodos()]
        |
        ↓
    [Abrir FormDashboard(lista)]
        |
        ↓
    [Calcular estadísticas]
        |
        Automóviles: Count(v is Automovil)
        Camiones: Count(v is Camion)
        Motocicletas: Count(v is Motocicleta)
        Valor flota: Sum(Precio - Depreciacion)
        Depreciación: Sum(CalcularDepreciacion)
        Mantenimiento: Sum(CalcularMantenimiento)
        |
        ↓
    [Mostrar tarjetas de resumen]
        |
        ↓
    [Mostrar tarjetas financieras]
        |
        ↓
    [Dibujar gráfica de barras con GDI+]
        |
        ↓
    [FIN]

---

## Flujo: Exportar Excel

    [Usuario presiona "Exportar Excel"]
        |
        ↓
    [ExportService.ExportarAExcel(lista)]
        |
        ↓
    [Crear XLWorkbook con ClosedXML]
        |
        ↓
    [Agregar encabezado principal]
        |
        ↓
    [Agregar encabezados de columnas con estilo]
        |
        ↓
    [Iterar vehículos y agregar filas]
        |
        ↓
    [Agregar fila de totales con fórmulas]
        |
        ↓
    [Ajustar columnas automáticamente]
        |
        ↓
    [Guardar en Escritorio con timestamp]
        |
        ↓
    [Mostrar ruta del archivo guardado]
        |
        ↓
    [FIN]