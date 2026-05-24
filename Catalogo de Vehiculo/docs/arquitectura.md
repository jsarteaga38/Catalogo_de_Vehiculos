# 🏗️ Justificación de Arquitectura

## Proyecto: Catálogo de Vehículos
**Materia:** Herramientas de Programación  
**Institución:** Institución Universitaria Pascual Bravo  
**Integrantes:** Juan Sebastián Guillén Arteaga · Juan Esteban Castaño Montoya

---

## Arquitectura Seleccionada: Clean Architecture

### ¿Qué es Clean Architecture?
Clean Architecture es un estilo arquitectónico propuesto por Robert C. Martin (Uncle Bob) que organiza el código en capas concéntricas donde las dependencias siempre apuntan hacia adentro — las capas externas conocen a las internas, pero nunca al revés.

---

## ¿Por qué Clean Architecture para este proyecto?

### 1. Separación clara de responsabilidades
El proyecto maneja tres preocupaciones distintas que no deben mezclarse:
- **Reglas de negocio** (cómo se calcula la depreciación)
- **Acceso a datos** (cómo se persiste en SQL Server)
- **Interfaz gráfica** (cómo se muestra al usuario)

Clean Architecture garantiza que cada preocupación viva en su propia capa sin contaminar las demás.

### 2. Independencia de la base de datos
Con Clean Architecture, si mañana se decide cambiar SQL Server por MySQL o por un archivo JSON, solo se modifica la capa Infrastructure. El dominio y la lógica de negocio no se tocan.

    Antes (sin arquitectura):
    FormPrincipal → SQL Server directamente
    Problema: cambiar BD = reescribir el formulario

    Después (con Clean Architecture):
    FormPrincipal → CatalogoService → IVehiculoRepository → SQL Server
    Solución: cambiar BD = solo modificar Infrastructure

### 3. Independencia de la interfaz gráfica
Gracias al patrón MVP integrado en la arquitectura, la lógica de negocio no depende de WinForms. Si se decidiera migrar a una aplicación web, solo se reemplaza la capa Presentation.

### 4. Testabilidad
Cada capa puede probarse de forma independiente:
- Domain: pruebas unitarias de depreciación sin BD
- Application: pruebas con repositorios mock
- Infrastructure: pruebas de integración con BD real
- Presentation: pruebas de UI independientes

---

## Las 4 Capas del Proyecto

    +------------------------------------------+
    |            PRESENTATION                  |
    |   FormPrincipal, FormDashboard,          |
    |   CatalogoPresenter                      |
    |   → Conoce: Application, Domain         |
    +------------------------------------------+
                      |
                      ↓ depende de
    +------------------------------------------+
    |            APPLICATION                   |
    |   CatalogoService, ExportService         |
    |   → Conoce: Domain                       |
    +------------------------------------------+
                      |
                      ↓ depende de
    +------------------------------------------+
    |              DOMAIN                      |
    |   Vehiculo, Automovil, Camion,           |
    |   Motocicleta, Interfaces, Factory       |
    |   → No conoce a nadie                    |
    +------------------------------------------+
                      △
                      | implementa
    +------------------------------------------+
    |           INFRASTRUCTURE                 |
    |   VehiculoRepository, UnitOfWork,        |
    |   AutomovilRepository, etc.              |
    |   → Conoce: Domain (implementa interfaces)|
    +------------------------------------------+

---

## Regla de Dependencia

La regla fundamental es que el código fuente solo puede apuntar hacia adentro:

    Presentation → Application → Domain ← Infrastructure

    ✅ FormPrincipal conoce CatalogoService
    ✅ CatalogoService conoce IVehiculoRepository
    ✅ VehiculoRepository implementa IVehiculoRepository
    ❌ Domain NO conoce Infrastructure
    ❌ Domain NO conoce Presentation
    ❌ Application NO conoce Presentation

---

## Comparación con otras arquitecturas

| Característica | MVC | Capas tradicional | Clean Architecture |
|---------------|-----|-------------------|--------------------|
| Independencia de BD | ❌ | Parcial | ✅ |
| Independencia de UI | ❌ | Parcial | ✅ |
| Testabilidad | Parcial | Parcial | ✅ |
| Complejidad inicial | Baja | Media | Media-Alta |
| Mantenibilidad | Media | Media | Alta |
| Escalabilidad | Media | Media | Alta |

Para un proyecto universitario de mediana escala como este, Clean Architecture es la opción más adecuada porque:
- Demuestra dominio técnico de arquitectura de software
- Es justificable y documentable claramente
- Permite aplicar todos los patrones SOLID de forma natural
- El código resultante es limpio, organizado y profesional

---

## Evidencia en el Código

### Domain no depende de nada externo
    namespace Catalogo_de_Vehiculo.Domain.Entities
    // Solo usa System — sin referencias a SQL, WinForms o servicios

### Infrastructure depende solo de Domain
    using Catalogo_de_Vehiculo.Domain.Entities;
    using Catalogo_de_Vehiculo.Domain.Interfaces;
    // Sin referencias a Presentation o Application

### Presentation depende de Application y Domain
    using Catalogo_de_Vehiculo.Application.Services;
    using Catalogo_de_Vehiculo.Domain.Interfaces;
    // Sin SQL directo, sin queries en el formulario

---

## Conclusión

Clean Architecture fue la decisión correcta para este proyecto porque resolvió los tres problemas principales:

1. **Acoplamiento** — antes el formulario tenía SQL directo; ahora hay capas claras
2. **Responsabilidad** — cada clase sabe exactamente qué debe hacer
3. **Extensibilidad** — agregar nuevos tipos de vehículos o cambiar la BD no rompe nada

La combinación de Clean Architecture con los patrones Factory Method, Repository+UoW y Observer, junto con los principios SOLID, produce un sistema robusto, mantenible y bien documentado.