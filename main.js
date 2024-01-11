import { dotnet } from './_framework/dotnet.js'

const { getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .create();

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

console.log("config.mainAssemblyName -> " + config.mainAssemblyName);

dotnet.instance.Module['canvas'] = document.getElementById('canvas');

function mainLoop() {
    exports.RhythmGalaxyWasm.Application.UpdateFrame();

    window.requestAnimationFrame(mainLoop);
}

await dotnet.run();
window.requestAnimationFrame(mainLoop);