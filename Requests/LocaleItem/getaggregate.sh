#!/bin/bash
curl -X GET http://localhost:5028/localeitem/detail/596a106d-f8cb-4dd3-b062-a7b40bfc5f92 \
   --header "Content-Type: application/json" \
   --data "$json_data"  | jq .

