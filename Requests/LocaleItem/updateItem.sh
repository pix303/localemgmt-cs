#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "ciao amore 34535",
    "context": "app",
    "userId": "0",
    "aggregateId": "0f3d8c94-5bca-43bd-a6dc-e4dcf02482dc"
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
    "content": "ciao amore 88",
    "context": "appero",
    "userId": "0",
    "aggregateId": "0f3d8c94-5bca-43bd-a6dc-e4dcf02482dc"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

