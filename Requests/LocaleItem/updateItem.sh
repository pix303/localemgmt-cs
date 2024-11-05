#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "questo ultimo inserimento in update",
    "context": "app",
    "userId": "123",
    "aggregateId": "131f1a32-e55c-42f9-afd0-e754d22977cb"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .



json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "questo veramente ultimo inserimento in update",
    "context": "appero",
    "userId": "1230",
    "aggregateId": "131f1a32-e55c-42f9-afd0-e754d22977cb"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

