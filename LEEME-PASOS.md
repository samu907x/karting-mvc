# Seguimiento 3 — ASP.NET Core MVC, Git/GitHub y publicación

Requisitos en tu computador: **.NET SDK**, **Git** y **VS Code** (con la extensión C# Dev Kit). Comprueba con:

```
dotnet --version
git --version
```

---

## PARTE 1 — Abrir el proyecto (ya viene completo)

La carpeta `KartingMvc` ya trae todo: `.csproj`, `Program.cs`, modelos, controladores, vistas, `wwwroot` con Bootstrap y jQuery, `Dockerfile` y `.gitignore`. **No necesitas `dotnet new mvc`.**

```
cd KartingMvc
code .
dotnet run
```

Requiere el **.NET SDK 10** (el proyecto usa `net10.0`). Si tienes otra versión, cambia `<TargetFramework>` en `KartingMvc.csproj` y los dos `10.0` del `Dockerfile`.

## PARTE 2 — Commits por etapas

Para que GitHub muestre un historial real, sube el proyecto en varios commits, agregando cada grupo de archivos por separado:

```
git init
git add KartingMvc.csproj Program.cs appsettings.json appsettings.Development.json Properties .gitignore
git commit -m "Proyecto ASP.NET Core MVC inicial"

git add Models Data
git commit -m "Agrega modelo Piloto y repositorio en memoria"

git add Controllers
git commit -m "Agrega controladores Home y Pilotos"

git add wwwroot Views/_ViewImports.cshtml Views/_ViewStart.cshtml Views/Shared
git commit -m "Agrega layout, estilos y librerias de wwwroot"

git add Views
git commit -m "Agrega vistas de inicio, posiciones, inscripcion y resultados"

git add Dockerfile LEEME-PASOS.md
git commit -m "Agrega Dockerfile y guia de publicacion"
```

Verifica con `git log --oneline` que aparezcan los 6 commits.

## PARTE 3 — Ejecutar y probar

```
dotnet run
```

Abre la dirección que aparece en la terminal (algo como `https://localhost:5xxx`). Prueba:

- Inicio, Posiciones, Inscripción y Resultados desde el menú.
- Enviar el formulario vacío: deben salir mensajes de validación.
- Inscribir un piloto, registrarle un resultado, filtrar por categoría y quitarlo.

Si hay errores de compilación, copia el mensaje completo y pégalo en el chat para que lo corrijamos.

## PARTE 4 — Subir a GitHub

1. Crea una cuenta en https://github.com si no tienes.
2. Clic en **New repository**. Nombre: `karting-mvc`. Déjalo **Public** y **sin** README ni .gitignore (ya los tienes).
3. Copia los comandos que GitHub te muestra y ejecútalos en la terminal:

```
git branch -M main
git remote add origin https://github.com/TU-USUARIO/karting-mvc.git
git push -u origin main
```

Recarga la página del repositorio: debes ver tus archivos y el historial de commits (botón **Commits**). Si git pide iniciar sesión, usa el navegador cuando lo ofrezca.

Para el video, practica estos tres conceptos:

- **Commit:** guarda una "foto" de los cambios en tu computador (`git commit`).
- **Push:** envía tus commits al repositorio remoto en GitHub (`git push`).
- **Pull:** trae a tu computador los cambios que hay en GitHub (`git pull`).

Demostración de Pull fácil: en GitHub, edita cualquier archivo (por ejemplo, crea un `README.md` desde el botón **Add file**) y haz commit ahí. Luego, en tu terminal, ejecuta `git pull` y muestra que el archivo apareció.

## PARTE 5 — Publicar en internet

Necesitas un servicio que ejecute .NET. Una opción gratuita habitual es **Render** usando el `Dockerfile` incluido (verifica en su página que el plan gratuito siga disponible; si no, alternativas son Azure App Service con la cuenta de estudiante, Railway o Fly.io).

**Antes de subir:** abre `KartingMvc.csproj`, mira la línea `<TargetFramework>` (por ejemplo `net8.0`) y en el `Dockerfile` cambia los dos `8.0` por tu versión (`9.0`, `10.0`, etc.). Haz commit y push de ese cambio.

**En Render:**

1. Entra a https://render.com e inicia sesión con tu cuenta de GitHub.
2. **New +** → **Web Service** → elige tu repositorio `karting-mvc`.
3. En **Language/Runtime** elige **Docker** (detecta el `Dockerfile` solo).
4. Plan: **Free**. Clic en **Create Web Service**.
5. Espera a que termine el despliegue (varios minutos). Arriba verás la URL pública, algo como `https://karting-mvc.onrender.com`.

Notas:

- En el plan gratuito la app "se duerme" tras un rato sin visitas y la primera carga puede tardar casi un minuto. **Ábrela antes de grabar el video** y antes de la entrega.
- Los pilotos viven en memoria: al reiniciarse la app vuelven a los 5 iniciales. Es normal para este seguimiento.
- Cada `git push` a `main` vuelve a desplegar automáticamente.

## PARTE 6 — Qué entregar

1. Enlace del video en Teams (con permisos para el profesor).
2. Enlace del repositorio de GitHub.
3. URL pública de la aplicación.
4. Envíalos también al correo que indique el docente.

---

## Cómo se relacionan las piezas (para explicar en el video)

- **Modelo** (`Models/Piloto.cs`): define qué es un piloto (propiedades y tipos) y sus reglas de validación.
- **Controlador** (`Controllers/PilotosController.cs`): recibe la solicitud, usa el modelo y decide qué vista devolver.
- **Vista** (`Views/Pilotos/*.cshtml`): HTML + Razor + Bootstrap que muestra los datos al usuario.
- **wwwroot**: archivos estáticos (CSS, JS, librerías) que el navegador descarga directamente.
- **Data** (`RepositorioPilotos.cs`): lista en memoria que hace de "base de datos".

**Recorrido de una solicitud:** el navegador pide `/Pilotos/Index` → el *routing* lee el patrón `{controlador}/{acción}/{id?}` → se ejecuta `PilotosController.Index` → el controlador pide los datos al repositorio → devuelve `View(pilotos)` → Razor combina `Views/Pilotos/Index.cshtml` con el layout y genera el HTML → el navegador recibe y muestra la respuesta.

**GET y POST:** `GET /Pilotos/Create` muestra el formulario vacío. Al enviarlo, el navegador hace `POST /Pilotos/Create`; el controlador recibe un objeto `Piloto`, valida con `ModelState.IsValid`, lo guarda y redirige a la lista (patrón Post-Redirect-Get).
