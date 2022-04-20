#pragma once
#include <string>

class Util {
public:
    static std::string GetConcatPath(const std::string& first, const std::string& second) {
        std::string temp = first;

        if (first[first.length()] != '\\') {
            temp += '\\';

            return temp + second;
        } else {
            return first + second;
        }
    }

    static std::string GetDirPath(const std::string& fileName) {
        const size_t last = fileName.rfind('\\');
        if (std::string::npos != last) {
            return fileName.substr(0, last);
        }
        return std::string();
    }

    // FIXME: Hardcoded.
    static std::string GetModulePath() {
        char path[0x800];
        GetModuleFileNameA(GetModuleHandleA("Reality.Bootstrap.dll"), path, 0x800);
        return path;
    }

    static std::wstring ToUtf16(const std::string& value, int codePage) {
        if (value.empty()) {
            return std::wstring();
        }
        int size = MultiByteToWideChar(codePage, 0, &value[0], (int) value.size(), 0, 0);
        std::wstring result(size, 0);
        MultiByteToWideChar(codePage, 0, &value[0], (int) value.size(), &result[0], size);
        return result;
    }
};
