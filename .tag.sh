#!/usr/bin/env bash

git checkout master --
git pull -v --progress "origin"
version=v$(head -n 1 .version)
git tag -a $version -m "Очередное обновление функционала."
git push origin refs/tags/$version