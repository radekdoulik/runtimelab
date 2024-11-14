#!/bin/sh

set -ex

# This script will regenerate the `wit-bindgen`-generated files in this
# directory.

# Prerequisites:
#   POSIX shell
#   tar
#   [cargo](https://rustup.rs/)
#   [curl](https://curl.se/download.html)

cargo install --locked --no-default-features --features csharp --version 0.32.0 wit-bindgen-cli
wit-bindgen c-sharp -w library -r native-aot wit
rm LibraryWorld_wasm_import_linkage_attribute.cs
