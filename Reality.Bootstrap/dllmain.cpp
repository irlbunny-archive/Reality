#include <windows.h>
#include <metahost.h>
#pragma comment(lib, "mscoree.lib")

#include "proxy.h"
#include "util.h"

int main() {
    std::string modulePath = Util::GetModulePath();
    std::string assemblyPath = Util::GetConcatPath(Util::GetDirPath(modulePath), "Reality.ModLoader.dll");

    ICLRMetaHost* metaHost;
    ICLRRuntimeInfo* runtimeInfo;
    ICLRRuntimeHost* runtimeHost;
    if (CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, reinterpret_cast<LPVOID*>(&metaHost)) == S_OK) {
        if (metaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, reinterpret_cast<LPVOID*>(&runtimeInfo)) == S_OK) {
            if (runtimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, reinterpret_cast<LPVOID*>(&runtimeHost)) == S_OK) {
                if (runtimeHost->Start() == S_OK) {
                    DWORD returnValue;
                    runtimeHost->ExecuteInDefaultAppDomain(Util::ToUtf16(assemblyPath, CP_UTF8).c_str(), L"Reality.ModLoader.Bootstrap", L"HostedMain", L"", &returnValue);
                }
            }
        }
    }

    return 0;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD dwReason, LPVOID lpReserved) {
    if (dwReason == DLL_PROCESS_ATTACH) {
        InitProxy();

        if (CreateThread(0, 0, reinterpret_cast<LPTHREAD_START_ROUTINE>(main), 0, 0, 0)) {
            return TRUE;
        } else {
            return FALSE;
        }
    }

    return TRUE;
}
