#!/usr/bin/env bash
set -euo pipefail

exec 3>&1 # keep near the start of the script
function say () {
  printf "%b\n" "[destroy] $1" >&3
}

function say_err() {
  if [ -t 1 ] && command -v tput > /dev/null; then
    RED='\033[0;31m'
    NC='\033[0m' # No Color
  fi
  printf "%b\n" "${RED:-}[destroy] Error: $1${NC:-}" >&2
}

# The full path to this script. https://stackoverflow.com/a/246128
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
pushd "${SCRIPT_DIR}" > /dev/null

say "Destroying infrastructure via docker compose down..."
PH="not-used" docker compose -f docker-compose-infra.yml down -v
