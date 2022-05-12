#pragma once
#include <windows.h>
#include "util.h"

typedef DWORD (WINAPI* ORIG_VerFindFileA)(DWORD, LPCSTR, LPCSTR, LPCSTR, LPSTR, PUINT, LPSTR, PUINT);
ORIG_VerFindFileA o_VerFindFileA;
DWORD WINAPI VerFindFileA(DWORD uFlags, LPCSTR szFileName, LPCSTR szWinDir, LPCSTR szAppDir, LPSTR szCurDir, PUINT puCurDirLen, LPSTR szDestDir, PUINT puDestDirLen) {
    return (o_VerFindFileA)(uFlags, szFileName, szWinDir, szAppDir, szCurDir, puCurDirLen, szDestDir, puDestDirLen);
}

typedef DWORD (WINAPI* ORIG_VerFindFileW)(DWORD, LPCWSTR, LPCWSTR, LPCWSTR, LPWSTR, PUINT, LPWSTR, PUINT);
ORIG_VerFindFileW o_VerFindFileW;
DWORD WINAPI VerFindFileW(DWORD uFlags, LPCWSTR szFileName, LPCWSTR szWinDir, LPCWSTR szAppDir, LPWSTR szCurDir, PUINT puCurDirLen, LPWSTR szDestDir, PUINT puDestDirLen) {
    return (o_VerFindFileW)(uFlags, szFileName, szWinDir, szAppDir, szCurDir, puCurDirLen, szDestDir, puDestDirLen);
}

typedef DWORD (WINAPI* ORIG_VerInstallFileA)(DWORD, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPCSTR, LPSTR, PUINT);
ORIG_VerInstallFileA o_VerInstallFileA;
DWORD WINAPI VerInstallFileA(DWORD uFlags, LPCSTR szSrcFileName, LPCSTR szDestFileName, LPCSTR szSrcDir, LPCSTR szDestDir, LPCSTR szCurDir, LPSTR szTmpFile, PUINT puTmpFileLen) {
    return (o_VerInstallFileA)(uFlags, szSrcFileName, szDestFileName, szSrcDir, szDestDir, szCurDir, szTmpFile, puTmpFileLen);
}

typedef DWORD (WINAPI* ORIG_VerInstallFileW)(DWORD, LPCWSTR, LPCWSTR, LPCWSTR, LPCWSTR, LPCWSTR, LPWSTR, PUINT);
ORIG_VerInstallFileW o_VerInstallFileW;
DWORD WINAPI VerInstallFileW(DWORD uFlags, LPCWSTR szSrcFileName, LPCWSTR szDestFileName, LPCWSTR szSrcDir, LPCWSTR szDestDir, LPCWSTR szCurDir, LPWSTR szTmpFile, PUINT puTmpFileLen) {
    return (o_VerInstallFileW)(uFlags, szSrcFileName, szDestFileName, szSrcDir, szDestDir, szCurDir, szTmpFile, puTmpFileLen);
}

typedef DWORD (WINAPI* ORIG_GetFileVersionInfoSizeA)(LPCSTR, LPDWORD);
ORIG_GetFileVersionInfoSizeA o_GetFileVersionInfoSizeA;
DWORD WINAPI GetFileVersionInfoSizeA(LPCSTR lptstrFilename, LPDWORD lpdwHandle) {
    return (o_GetFileVersionInfoSizeA)(lptstrFilename, lpdwHandle);
}

typedef DWORD (WINAPI* ORIG_GetFileVersionInfoSizeW)(LPCWSTR, LPDWORD);
ORIG_GetFileVersionInfoSizeW o_GetFileVersionInfoSizeW;
DWORD WINAPI GetFileVersionInfoSizeW(LPCWSTR lptstrFilename, LPDWORD lpdwHandle) {
    return (o_GetFileVersionInfoSizeW)(lptstrFilename, lpdwHandle);
}

typedef DWORD (WINAPI* ORIG_GetFileVersionInfoA)(LPCSTR, DWORD, DWORD, LPVOID);
ORIG_GetFileVersionInfoA o_GetFileVersionInfoA;
BOOL WINAPI GetFileVersionInfoA(LPCSTR lptstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData) {
    return (o_GetFileVersionInfoA)(lptstrFilename, dwHandle, dwLen, lpData);
}

typedef DWORD (WINAPI* ORIG_GetFileVersionInfoW)(LPCWSTR, DWORD, DWORD, LPVOID);
ORIG_GetFileVersionInfoW o_GetFileVersionInfoW;
BOOL WINAPI GetFileVersionInfoW(LPCWSTR lptstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData) {
    return (o_GetFileVersionInfoW)(lptstrFilename, dwHandle, dwLen, lpData);
}

typedef DWORD (WINAPI* ORIG_GetFileVersionInfoSizeExA)(DWORD, LPCSTR, LPDWORD);
ORIG_GetFileVersionInfoSizeExA o_GetFileVersionInfoSizeExA;
DWORD WINAPI GetFileVersionInfoSizeExA(DWORD dwFlags, LPCSTR lpwstrFilename, LPDWORD lpdwHandle) {
    return (o_GetFileVersionInfoSizeExA)(dwFlags, lpwstrFilename, lpdwHandle);
}

typedef DWORD (WINAPI* ORIG_GetFileVersionInfoSizeExW)(DWORD, LPCWSTR, LPDWORD);
ORIG_GetFileVersionInfoSizeExW o_GetFileVersionInfoSizeExW;
DWORD WINAPI GetFileVersionInfoSizeExW(DWORD dwFlags, LPCWSTR lpwstrFilename, LPDWORD lpdwHandle) {
    return (o_GetFileVersionInfoSizeExW)(dwFlags, lpwstrFilename, lpdwHandle);
}

typedef DWORD (WINAPI* ORIG_GetFileVersionInfoExA)(DWORD, LPCSTR, DWORD, DWORD, LPVOID);
ORIG_GetFileVersionInfoExA o_GetFileVersionInfoExA;
BOOL WINAPI GetFileVersionInfoExA(DWORD dwFlags, LPCSTR lpwstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData) {
    return (o_GetFileVersionInfoExA)(dwFlags, lpwstrFilename, dwHandle, dwLen, lpData);
}

typedef DWORD (WINAPI* ORIG_GetFileVersionInfoExW)(DWORD, LPCWSTR, DWORD, DWORD, LPVOID);
ORIG_GetFileVersionInfoExW o_GetFileVersionInfoExW;
BOOL WINAPI GetFileVersionInfoExW(DWORD dwFlags, LPCWSTR lpwstrFilename, DWORD dwHandle, DWORD dwLen, LPVOID lpData) {
    return (o_GetFileVersionInfoExW)(dwFlags, lpwstrFilename, dwHandle, dwLen, lpData);
}

typedef DWORD (WINAPI* ORIG_VerLanguageNameA)(DWORD, LPSTR, DWORD);
ORIG_VerLanguageNameA o_VerLanguageNameA;
DWORD WINAPI VerLanguageNameA(DWORD wLang, LPSTR szLang, DWORD cchLang) {
    return (o_VerLanguageNameA)(wLang, szLang, cchLang);
}

typedef DWORD (WINAPI* ORIG_VerLanguageNameW)(DWORD, LPWSTR, DWORD);
ORIG_VerLanguageNameW o_VerLanguageNameW;
DWORD WINAPI VerLanguageNameW(DWORD wLang, LPWSTR szLang, DWORD cchLang) {
    return (o_VerLanguageNameW)(wLang, szLang, cchLang);
}

typedef DWORD (WINAPI* ORIG_VerQueryValueA)(LPCVOID, LPCSTR, LPVOID*, PUINT);
ORIG_VerQueryValueA o_VerQueryValueA;
BOOL WINAPI VerQueryValueA(LPCVOID pBlock, LPCSTR lpSubBlock, LPVOID* lplpBuffer, PUINT puLen) {
    return (o_VerQueryValueA)(pBlock, lpSubBlock, lplpBuffer, puLen);
}

typedef DWORD (WINAPI* ORIG_VerQueryValueW)(LPCVOID, LPCWSTR, LPVOID*, PUINT);
ORIG_VerQueryValueW o_VerQueryValueW;
BOOL WINAPI VerQueryValueW(LPCVOID pBlock, LPCWSTR lpSubBlock, LPVOID* lplpBuffer, PUINT puLen) {
    return (o_VerQueryValueW)(pBlock, lpSubBlock, lplpBuffer, puLen);
}

typedef void (WINAPI* ORIG_GetFileVersionInfoByHandle)();
ORIG_GetFileVersionInfoByHandle o_GetFileVersionInfoByHandle;
void WINAPI GetFileVersionInfoByHandle() {
    (o_GetFileVersionInfoByHandle)();
}

void InitProxy() {
	HMODULE h_OriginalLibrary = LoadLibraryA(Util::GetConcatPath(Util::GetSysDirPath(), "version.dll").c_str());
	o_VerFindFileA = (ORIG_VerFindFileA) GetProcAddress(h_OriginalLibrary, "VerFindFileA");
	o_VerFindFileW = (ORIG_VerFindFileW) GetProcAddress(h_OriginalLibrary, "VerFindFileW");
	o_VerInstallFileA = (ORIG_VerInstallFileA) GetProcAddress(h_OriginalLibrary, "VerInstallFileA");
	o_VerInstallFileW = (ORIG_VerInstallFileW) GetProcAddress(h_OriginalLibrary, "VerInstallFileW");
	o_GetFileVersionInfoA = (ORIG_GetFileVersionInfoA) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoA");
	o_GetFileVersionInfoW = (ORIG_GetFileVersionInfoW) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoW");
	o_GetFileVersionInfoSizeA = (ORIG_GetFileVersionInfoSizeA) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoSizeA");
	o_GetFileVersionInfoSizeW = (ORIG_GetFileVersionInfoSizeW) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoSizeW");
	o_GetFileVersionInfoExA = (ORIG_GetFileVersionInfoExA) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoExA");
	o_GetFileVersionInfoExW = (ORIG_GetFileVersionInfoExW) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoExW");
	o_GetFileVersionInfoSizeExA = (ORIG_GetFileVersionInfoSizeExA) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoSizeExA");
	o_GetFileVersionInfoSizeExW = (ORIG_GetFileVersionInfoSizeExW) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoSizeExW");
	o_VerLanguageNameA = (ORIG_VerLanguageNameA) GetProcAddress(h_OriginalLibrary, "VerLanguageNameA");
	o_VerLanguageNameW = (ORIG_VerLanguageNameW) GetProcAddress(h_OriginalLibrary, "VerLanguageNameW");
	o_VerQueryValueA = (ORIG_VerQueryValueA) GetProcAddress(h_OriginalLibrary, "VerQueryValueA");
	o_VerQueryValueW = (ORIG_VerQueryValueW) GetProcAddress(h_OriginalLibrary, "VerQueryValueW");
	o_GetFileVersionInfoByHandle = (ORIG_GetFileVersionInfoByHandle) GetProcAddress(h_OriginalLibrary, "GetFileVersionInfoByHandle");
}
