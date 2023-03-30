#!/bin/bash
set -eu

VERSION="${1}"
# sed -i -b -e "s/\[Unreleased\]/\[${VERSION}\] - $(date -I)/g" CHANGELOG*.md
sed -i -b -e "s/\"version\": \".*\"/\"version\": \"${VERSION}\"/g" package.json
sed -i -b -e "s|download/.*.zip|download/v${VERSION}/com.github.kurotu.krt-avatar-utility-${VERSION}.zip|g" package.json
# sed -i -b -e "s/string Version = \".*\"/string Version = \"${VERSION}\"/g" Editor/KRTAvatarUtility.cs
git commit -am "Version ${VERSION}"
git tag "v${VERSION}"
