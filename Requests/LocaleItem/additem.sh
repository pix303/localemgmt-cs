#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "ciao amore",
    "context": "app",
    "userId": "0"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/add/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .
