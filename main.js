import { dotnet } from './_framework/dotnet.js'

const { getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .create();

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

console.log("config.mainAssemblyName -> " + config.mainAssemblyName);
console.log("exports -> " + exports);

dotnet.instance.Module['canvas'] = document.getElementById('canvas');

function mainLoop() {
    exports.RhythmGalaxy.Application.Update();

    window.requestAnimationFrame(mainLoop);
}

await dotnet.run();
window.requestAnimationFrame(mainLoop);