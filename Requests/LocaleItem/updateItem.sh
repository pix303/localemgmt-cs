#!/bin/bash

json_data=$(
  cat <<EOF
{
    "lang": "it",
    "content": "ciao amore",
    "context": "app",
    "userId": "0",
    "aggregateId": "cff9170d-d186-4f6f-acd6-1a2b3729e664"
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
    "aggregateId": "cff9170d-d186-4f6f-acd6-1a2b3729e664"
}
EOF
)

curl -X POST http://localhost:5028/localeitem/update/ \
  --header "Content-Type: application/json" \
  --data "$json_data"  | jq .

